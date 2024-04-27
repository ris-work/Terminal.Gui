﻿// ReSharper disable EnumUnderlyingTypeIsInt
using Terminal.Gui.Analyzers.Internal.Attributes;

namespace Terminal.Gui.Analyzers.Internal.Tests.Generators.EnumExtensions.EnumDefinitions;

/// <summary>
///     Same as <see cref="BetterEnum_ExplicitInt"/>, but with <see cref="GenerateEnumExtensionMethodsAttribute.FastIsDefined"/> = <see langword="false" />.
/// </summary>
[GenerateEnumExtensionMethods (FastIsDefined = false)]
public enum BetterEnum_ExplicitInt_NoFastIsDefined : int
{
    Bit31 = -0b_10000000_00000000_00000000_00000000,
    Bit30 = 0b_01000000_00000000_00000000_00000000,
    Bit29 = 0b_00100000_00000000_00000000_00000000,
    Bit28 = 0b_00010000_00000000_00000000_00000000,
    Bit27 = 0b_00001000_00000000_00000000_00000000,
    Bit26 = 0b_00000100_00000000_00000000_00000000,
    Bit25 = 0b_00000010_00000000_00000000_00000000,
    Bit24 = 0b_00000001_00000000_00000000_00000000,
    Bit23 = 0b_00000000_10000000_00000000_00000000,
    Bit22 = 0b_00000000_01000000_00000000_00000000,
    Bit21 = 0b_00000000_00100000_00000000_00000000,
    Bit20 = 0b_00000000_00010000_00000000_00000000,
    Bit19 = 0b_00000000_00001000_00000000_00000000,
    Bit18 = 0b_00000000_00000100_00000000_00000000,
    Bit17 = 0b_00000000_00000010_00000000_00000000,
    Bit16 = 0b_00000000_00000001_00000000_00000000,
    Bit15 = 0b_00000000_00000000_10000000_00000000,
    Bit14 = 0b_00000000_00000000_01000000_00000000,
    Bit13 = 0b_00000000_00000000_00100000_00000000,
    Bit12 = 0b_00000000_00000000_00010000_00000000,
    Bit11 = 0b_00000000_00000000_00001000_00000000,
    Bit10 = 0b_00000000_00000000_00000100_00000000,
    Bit09 = 0b_00000000_00000000_00000010_00000000,
    Bit08 = 0b_00000000_00000000_00000001_00000000,
    Bit07 = 0b_00000000_00000000_00000000_10000000,
    Bit06 = 0b_00000000_00000000_00000000_01000000,
    Bit05 = 0b_00000000_00000000_00000000_00100000,
    Bit04 = 0b_00000000_00000000_00000000_00010000,
    Bit03 = 0b_00000000_00000000_00000000_00001000,
    Bit02 = 0b_00000000_00000000_00000000_00000100,
    Bit01 = 0b_00000000_00000000_00000000_00000010,
    Bit00 = 0b_00000000_00000000_00000000_00000001,
    All_0  =  0,
    All_1  = ~All_0,
    Alternating_01 = 0b_01010101_01010101_01010101_01010101,
    Alternating_10 = ~Alternating_01,
    EvenBytesHigh = 0b_00000000_11111111_00000000_11111111,
    OddBytesHigh = ~EvenBytesHigh,
}
