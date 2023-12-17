using BrainlessPuzzleGameResolver.Structs;
using System.Runtime.InteropServices;

namespace BrainlessPuzzleGameResolver;

internal partial class Program
{
    private static void Main()
    {
    Restart:
        Console.Clear();
        Console.WriteLine("Type the text you want to be converted to keyboard inputs (works with '↓←↑→' too)\nIf you ONLY want to paste ↓←↑→, you have to type a '/' as the FIRST input, such as /↓←↑→) :");
        string? l_RawTextInput;

        while (true)
        {
            l_RawTextInput = Console.ReadLine();

            if (string.IsNullOrEmpty(l_RawTextInput))
                Console.WriteLine("You cannot send empty inputs");
            else
            {
                if (l_RawTextInput[0] == '/')
                    l_RawTextInput = l_RawTextInput[1..];
                break;
            }
        }

        Console.WriteLine("Type the delay between each input (in milliseconds):");
        int? l_TimeBetweenKeys;

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out var l_ParsedInt))
            {
                l_TimeBetweenKeys = l_ParsedInt;
                break;
            }

            Console.WriteLine("You must enter a valid integer");
        }

        Console.WriteLine("Do you want to remove the spaces from the input? (y/n)");
        bool? l_RemoveSpaces;

        while (true)
        {
            l_RemoveSpaces = Console.ReadLine()?.ToLower() switch
            {
                "y" => true,
                "n" => false,
                _   => null
            };

            if (l_RemoveSpaces is null)
                Console.WriteLine("You must enter 'y' or 'n'");
            else
                break;
        }

        if (l_RemoveSpaces.Value)
            l_RawTextInput = l_RawTextInput.Replace(" ", "");

        Console.WriteLine("\nSending you inputs in 2 seconds..");
        Thread.Sleep(2000);

        foreach (var l_Char in l_RawTextInput)
        {
            var l_NewChar = char.ToUpper(l_Char);
            var l_Inputs  = InputParser(l_NewChar);

            for (var l_Index = 0; l_Index < 2; l_Index++)
            {
                SendInput(1, new[]
                {
                    l_Inputs[l_Index]
                }, Marshal.SizeOf(typeof(Input)));
                Thread.Sleep(25);
            }

            Thread.Sleep(l_TimeBetweenKeys.Value);
        }

        Console.WriteLine("Input has been sent, press \"y\" to input other instructions, or simply any other key to close the program.");
        if (Console.ReadLine() is "y" or "Y")
            goto Restart;
    }

    private static Input[] InputParser(char p_Char)
    {
        var l_InputsArray = new Input[2];

        var l_ParsedScanCode = ParseKeyScanCode(p_Char);
        l_InputsArray[0] = new Input
        {
            type = (int)InputType.Keyboard,
            u = new InputUnion
            {
                ki = new KeyboardInput
                {
                    wVk         = l_ParsedScanCode,
                    wScan       = 0,
                    dwFlags     = 0,
                    dwExtraInfo = IntPtr.Zero,
                    time        = 0
                }
            }
        };
        l_InputsArray[1] =
            new Input
            {
                type = (int)InputType.Keyboard,
                u = new InputUnion
                {
                    ki = new KeyboardInput
                    {
                        wVk         = l_ParsedScanCode,
                        wScan       = 0,
                        dwFlags     = 2,
                        dwExtraInfo = IntPtr.Zero,
                        time        = 0
                    }
                }
            };

        return l_InputsArray;
    }

    private static ushort ParseKeyScanCode(char p_Char)
    {
        /// Feed the switch expression with DirectInput KeyCode Table characters
        return p_Char switch
        {
            (char)25 => 40,   // '↓'
            (char)26 => 0x27, // '→'
            (char)27 => 0x25, // '←'
            (char)24 => 0x26, // '↑'
            ' '      => 0x20, // 'Space'
            '↑'      => 0x26,
            '←'      => 0x25,
            '↓'      => 40,
            '→'      => 0x27,
            '1'      => 0x61,
            '2'      => 0x62,
            '3'      => 0x63,
            '4'      => 100,
            '5'      => 0x65,
            '6'      => 0x66,
            '7'      => 0x67,
            '8'      => 0x68,
            '9'      => 0x69,
            '0'      => 0x60,
            'A'      => 0x41,
            'B'      => 0x42,
            'C'      => 0x43,
            'D'      => 0x44,
            'E'      => 0x45,
            'F'      => 70,
            'G'      => 0x47,
            'H'      => 0x48,
            'I'      => 0x49,
            'J'      => 0x4A,
            'K'      => 0x4B,
            'L'      => 0x4C,
            'M'      => 0x4D,
            'N'      => 0x4E,
            'O'      => 0x4F,
            'P'      => 80,
            'Q'      => 0x51,
            'R'      => 0x52,
            'S'      => 0x53,
            'T'      => 0x54,
            'U'      => 0x55,
            'V'      => 0x56,
            'W'      => 0x57,
            'X'      => 0x58,
            'Y'      => 0x59,
            'Z'      => 90,
            _        => 0
        };
    }
}
