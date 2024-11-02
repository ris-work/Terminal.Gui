﻿using System.Text.RegularExpressions;

namespace Terminal.Gui;

/// <summary>
///     Uses Ansi escape sequences to detect whether sixel is supported
///     by the terminal.
/// </summary>
public class SixelSupportDetector
{
    /// <summary>
    /// Sends Ansi escape sequences to the console to determine whether
    /// sixel is supported (and <see cref="SixelSupportResult.Resolution"/>
    /// etc).
    /// </summary>
    /// <returns>Description of sixel support, may include assumptions where
    /// expected response codes are not returned by console.</returns>
    public void Detect (Action<SixelSupportResult> resultCallback)
    {
        var result = new SixelSupportResult ();
        result.SupportsTransparency = IsWindowsTerminal () || IsXtermWithTransparency ();
        IsSixelSupportedByDar (result, resultCallback);
    }


    private void TryGetResolutionDirectly (SixelSupportResult result, Action<SixelSupportResult> resultCallback)
    {
        // Expect something like:
        //<esc>[6;20;10t
        QueueRequest (EscSeqUtils.CSI_RequestSixelResolution,
                      (r) =>
                      {
                          // Terminal supports directly responding with resolution
                          var match = Regex.Match (r, @"\[\d+;(\d+);(\d+)t$");

                          if (match.Success)
                          {
                              if (int.TryParse (match.Groups [1].Value, out var ry) &&
                                  int.TryParse (match.Groups [2].Value, out var rx))
                              {
                                  result.Resolution = new Size (rx, ry);
                              }
                          }

                          // Finished
                          resultCallback.Invoke (result);

                      },
                      // Request failed, so try to compute instead
                      ()=>TryComputeResolution (result,resultCallback));
    }


    private void TryComputeResolution (SixelSupportResult result, Action<SixelSupportResult> resultCallback)
    {
        string windowSize;
        string sizeInChars;

        QueueRequest (EscSeqUtils.CSI_RequestWindowSizeInPixels,
                      (r1)=>
                      {
                          windowSize = r1;

                          QueueRequest (EscSeqUtils.CSI_ReportTerminalSizeInChars,
                                        (r2) =>
                                        {
                                            sizeInChars = r2;
                                            ComputeResolution (result,windowSize,sizeInChars);
                                            resultCallback (result);

                                        }, abandoned: () => resultCallback (result));
                      },abandoned: ()=>resultCallback(result));
    }

    private void ComputeResolution (SixelSupportResult result, string windowSize, string sizeInChars)
    {
        // Fallback to window size in pixels and characters
        // Example [4;600;1200t
        var pixelMatch = Regex.Match (windowSize, @"\[\d+;(\d+);(\d+)t$");

        // Example [8;30;120t
        var charMatch = Regex.Match (sizeInChars, @"\[\d+;(\d+);(\d+)t$");

        if (pixelMatch.Success && charMatch.Success)
        {
            // Extract pixel dimensions
            if (int.TryParse (pixelMatch.Groups [1].Value, out var pixelHeight)
                && int.TryParse (pixelMatch.Groups [2].Value, out var pixelWidth)
                &&

                // Extract character dimensions
                int.TryParse (charMatch.Groups [1].Value, out var charHeight)
                && int.TryParse (charMatch.Groups [2].Value, out var charWidth)
                && charWidth != 0
                && charHeight != 0) // Avoid divide by zero
            {
                // Calculate the character cell size in pixels
                var cellWidth = (int)Math.Round ((double)pixelWidth / charWidth);
                var cellHeight = (int)Math.Round ((double)pixelHeight / charHeight);

                // Set the resolution based on the character cell size
                result.Resolution = new Size (cellWidth, cellHeight);
            }
        }
    }

    private void IsSixelSupportedByDar (SixelSupportResult result,Action<SixelSupportResult> resultCallback)
    {
        QueueRequest (
              EscSeqUtils.CSI_SendDeviceAttributes,
              (r) =>
              {
                  result.IsSupported = ResponseIndicatesSupport (r);

                  if (result.IsSupported)
                  {
                      TryGetResolutionDirectly (result, resultCallback);
                  }
                  else
                  {
                      resultCallback (result);
                  }
              },abandoned: () => resultCallback(result));
    }

    private void QueueRequest (AnsiEscapeSequenceRequest req, Action<string> responseCallback, Action abandoned)
    {
        var newRequest = new AnsiEscapeSequenceRequest
        {
            Request = req.Request,
            Terminator = req.Terminator,
            ResponseReceived = responseCallback,
            Abandoned = abandoned
        };

        Application.Driver.QueueAnsiRequest (newRequest);
    }

    private bool ResponseIndicatesSupport (string response)
    {
        return response.Split (';').Contains ("4");
    }

    private bool IsWindowsTerminal ()
    {
        return  !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable ("WT_SESSION"));;
    }
    private bool IsXtermWithTransparency ()
    {
        // Check if running in real xterm (XTERM_VERSION is more reliable than TERM)
        var xtermVersionStr = Environment.GetEnvironmentVariable ("XTERM_VERSION");

        // If XTERM_VERSION exists, we are in a real xterm
        if (!string.IsNullOrWhiteSpace (xtermVersionStr) && int.TryParse (xtermVersionStr, out var xtermVersion) && xtermVersion >= 370)
        {
            return true;
        }

        return false;
    }
}