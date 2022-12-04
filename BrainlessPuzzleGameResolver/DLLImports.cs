using System.Runtime.InteropServices;
using BrainlessPuzzleGameResolver.Structs;

namespace BrainlessPuzzleGameResolver;

internal partial class Program
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);
}
