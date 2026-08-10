namespace SealPinTwinDemo;

/// <summary>
/// 电芯外形参数。毫米数据来自海辰 ∞Cell 1175Ah 官方规格书。
/// SceneMillimetersPerUnit 仅用于将真实尺寸映射到教学三维场景。
/// </summary>
public sealed record BatteryModelSpec(
    string Manufacturer,
    string ProductName,
    string Model,
    double CapacityAh,
    double NominalVoltage,
    double NominalEnergyWh,
    double LengthMm,
    double WidthMm,
    double HeightMm,
    double WeightKg)
{
    public const double SceneMillimetersPerUnit = 150.0;

    public double SceneLength => LengthMm / SceneMillimetersPerUnit;
    public double SceneWidth => WidthMm / SceneMillimetersPerUnit;
    public double SceneHeight => HeightMm / SceneMillimetersPerUnit;

    public static BatteryModelSpec HithiumInfinityCell1175Ah { get; } = new(
        Manufacturer: "海辰储能 HiTHIUM",
        ProductName: "∞Cell 1175Ah",
        Model: "MC1175P025A",
        CapacityAh: 1175,
        NominalVoltage: 3.2,
        NominalEnergyWh: 3760,
        LengthMm: 580.22,
        WidthMm: 75.22,
        HeightMm: 216.31,
        WeightKg: 20.4);
}
