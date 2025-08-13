using UnityEngine;

public static class Calculator
{
    public static int Calc(CalcEnum _calc, int a, int b)
    {
        switch (_calc)
        {
            case CalcEnum.Plus:
            case CalcEnum.Minus:
                return a + b;
            case CalcEnum.Multiple:
                return Mathf.RoundToInt((float)a * b);
            case CalcEnum.Divide:
                return Mathf.RoundToInt((float)a / b);
        }

        Debug.LogError("Can not find CalcEnum -> " + _calc);
        return 0;
    }

    public static int Calc(TileData _data, int a) => Calc(_data.calc, a, _data.num);
}
