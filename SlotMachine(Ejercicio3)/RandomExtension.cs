namespace SlotMachine_Ejercicio3_
{
    public static class RandomExtension
    {
        public static bool NextBool(this Random random)
        {
            return random.Next(2) == 0;
        }
    }
}
