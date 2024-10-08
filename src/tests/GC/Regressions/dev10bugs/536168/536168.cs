// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using System.Threading;
using Xunit;

public class Program
{
    private static volatile int g_completed = 0;
    private static int g_interations = 5000000;

    public void Test()
    {

        int?[] values = new int?[128 * 1024];

        for (int i = 0; i < values.Length; i++) values[i] = 5;

        for (int i = 0; i < values.Length; i++)
        {

            values[i] = (int)0x42424242;

            if (IsNull(values[i])) Console.WriteLine("Null found.");

        }

        System.Threading.Interlocked.Increment(ref g_completed);
    }

    public virtual bool IsNull(int? x)
    {

        return x == null;

    }

    [Fact]
    public static void TestEntryPoint()
    {

        Program p = new Program();

        for (int i = 0; i < g_interations; i++)
        {

            ThreadPool.QueueUserWorkItem(o => p.Test());

        }

        while (true)
        {
            GC.Collect();

            Thread.Sleep(1);

            if (g_completed >= g_interations) break; 
        }
    }
}
