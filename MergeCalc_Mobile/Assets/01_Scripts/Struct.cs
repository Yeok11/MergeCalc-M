public struct TileData
{
    public CalcEnum calc;
    public int num;
    public float time, bound;

    public TileData(CalcEnum _calc, int _int, float _t, float _b)
    {
        calc = _calc;
        num = _int;
        time = _t;
        bound = _b;

        if (calc == CalcEnum.Minus) num = -num;
    }
}
