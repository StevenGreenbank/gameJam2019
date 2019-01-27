using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    public string command;
    public string[] variables;

    public Instruction(string command, string parameter1): this(command, new string[] { parameter1 })
    {
    }

    public Instruction(string command, string parameter1, string parameter2)
        : this(command, new string[] { parameter1, parameter2 })
    {
    }

    public Instruction(string command, string[] parameters)
    {
        this.command = command;
        this.variables = parameters;
    }
}
