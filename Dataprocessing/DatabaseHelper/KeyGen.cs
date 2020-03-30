using System;
using System.Collections.Generic;
using System.Text;

// No, this is not a keygen for The Sims 2.
namespace DatabaseHelper
{
    /// <summary>
    /// Generates new simple API keys
    /// </summary>
    public class KeyGen
    {
        private const string CHARACTER_BAG = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@$%^&*";
        private const int KEY_LEN = 25;

        private Random rng;

        public KeyGen()
        {
            this.rng = new Random();
        }

        public string GetKey()
        {
            var key = new char[KEY_LEN];

            for(int i = 0; i < KEY_LEN; i++)
            {
                key[i] = CHARACTER_BAG[rng.Next(0, CHARACTER_BAG.Length)];
            }

            return new string(key);
        }
    }
}
