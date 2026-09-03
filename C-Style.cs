using System;

class AutomataTest
{
    private const int NUM_STATES = 5;

    // NFA Transition Function
    private static void GetTransitions(int state, char c, bool[] nextStateSet)
    {
        switch (state)
        {
            case 0:
                if (c == '/') nextStateSet[1] = true;
                break;

            case 1:
                if (c == '*') nextStateSet[2] = true;
                break;

            case 2:
                // NFA Nondeterminism: On '*', stay in q2 (body) AND move to q3 (closing candidate)
                if (c == '*')
                {
                    nextStateSet[2] = true;
                    nextStateSet[3] = true;
                }
                else if (c == 'a' || c == '/')
                {
                    nextStateSet[2] = true;
                }
                break;

            case 3:
                if (c == '/') nextStateSet[4] = true;
                break;

            case 4:
                break;
        }
    }

    private static bool IsAcceptedNFA(string input)
    {
        // Current set of active states (starts with state 0 active)
        bool[] activeStates = new bool[NUM_STATES] { true, false, false, false, false };

        foreach (char c in input)
        {
            bool[] nextActiveStates = new bool[NUM_STATES];

            // For every active state, evaluate all possible transitions for character 'c'
            for (int state = 0; state < NUM_STATES; state++)
            {
                if (activeStates[state])
                {
                    GetTransitions(state, c, nextActiveStates);
                }
            }

            // Copy next state set into active state set
            Array.Copy(nextActiveStates, activeStates, NUM_STATES);
        }

        // String is accepted if state 4 is present in the final active set
        return activeStates[4];
    }

    static void Main(string[] args)
    {
        Console.WriteLine("--- C-STYLE COMMENT MATCHING (C#) ---\n");

        string[] testCases = {
            "/*a*/",
            "/**/",
            "/***/",
            "/*aaa*aaa*/",
            "/*a/a*/",
            "/**",
            "/**/a/*aa*/",
            "aaa/**/a",
            "/*/",
            "/**a/",
            "//aaaa"
        };

        foreach (string test in testCases)
        {
            string status = IsAcceptedNFA(test) ? "ACCEPTED" : "REJECTED";
            if (test.Length < 8)
            {
                Console.WriteLine($"{test}\t\t-> {status}");
            }
            else
            {
                Console.WriteLine($"{test}\t-> {status}");
            }
        }

        Console.WriteLine("\n-----------------------------------");
        Console.Write("Enter custom string to test: ");

        string userInput = Console.ReadLine();
        if (userInput != null)
        {
            string result = IsAcceptedNFA(userInput) ? "ACCEPTED" : "REJECTED";
            Console.WriteLine($"Result: {result}");
        }
    }
}