public struct TileData
{
    public CalcEnum calc;
    public int num;

    public TileData(CalcEnum _calc, int _int)
    {
        calc = _calc;
        num = _int;

        if (calc == CalcEnum.Minus) num = -num;
    }
}
