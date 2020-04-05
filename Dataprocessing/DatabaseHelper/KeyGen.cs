using System;
using System.Collections.Generic;
using System.Text;

// No, this is not a keygen for The Sims 2.
// This class is left over from my simple API key system. 
// Remainders from that are seen in teh database as well.
namespace DatabaseHelper
{
    /// <summary>
    /// Generates new simple API keys
    /// </summary>
    public class KeyGen
    {
        private const string CHARACTER_BAG = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@$%^&*";
        private const int KEY_LEN = 25;

        private readonly Random rng;

        public KeyGen()
        {
            this.rng = new Random();
        }

        public string GetKey()
        {
            // create new array to hold characters
            var key = new char[KEY_LEN];

            for(int i = 0; i < KEY_LEN; i++)
            {
                // Get a random character from the source character bag
                key[i] = CHARACTER_BAG[rng.Next(0, CHARACTER_BAG.Length)];
            }

            // return api key
            return new string(key);
        }
    }
}
