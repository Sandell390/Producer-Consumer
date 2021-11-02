using System;
using System.Collections.Generic;
using System.Threading;

namespace Producer_Consumer
{
    class Program
    {

        private static Queue<int> buffer = new Queue<int>();

        static void Main(string[] args)
        {

            Thread producer = new Thread(FillBuffer);
            producer.Name = "Producer";
            producer.Start();

            Thread consumer = new Thread(ClearBuffer);
            consumer.Name = "Consumer";
            consumer.Start();

            Console.ReadLine();
        }
        static void FillBuffer()
        {
            while (true)
            {
                int[] newbuffer = new int[5];

                for (int i = 0; i < 5; i++)
                {
                    newbuffer[i] = i;
                }

                lock (buffer)
                {

                    while (buffer.Count > newbuffer.Length - 1)
                    {
                        Monitor.Wait(buffer);
                    }

                    for (int i = 0; i < newbuffer.Length; i++)
                    {
                        buffer.Enqueue(newbuffer[i]);
                        Console.WriteLine($"Producer index: {i}");
                        Thread.Sleep(500);
                    }

                    Monitor.PulseAll(buffer);
                }

                //     /\
                //     ||
                //     ||
                //Kan også gøres på denne måde. Så skifter producer og consumer hver gang der bliver lagt noget i buffer
                //for (int i = 0; i < newbuffer.Length; i++)
                //{
                //    lock (buffer)
                //    {

                //        while (buffer.Count > newbuffer.Length - 1)
                //        {
                //            Monitor.Wait(buffer);
                //        }

                //        buffer.Enqueue(newbuffer[i]);
                //        Console.WriteLine($"Producer index: {i}");
                //        Thread.Sleep(500);


                //        Monitor.PulseAll(buffer);
                //    }
                //}
            }
        }

        static void ClearBuffer()
        {
            while (true)
            {

                lock (buffer)
                {
                    while (buffer.Count == 0)
                    {
                        Monitor.Wait(buffer);
                    }

                    Console.WriteLine($"Consumer index: {buffer.Dequeue()}");
                    Thread.Sleep(500);

                    Monitor.PulseAll(buffer);
                }
            }
        }
    }
}
