public struct TileData
{
    public CalcEnum calc;
    public int num;
    public float time;

    public TileData(CalcEnum _calc, int _int, float _t)
    {
        calc = _calc;
        num = _int;
        time = _t;

        if (calc == CalcEnum.Minus) num = -num;
    }
}
