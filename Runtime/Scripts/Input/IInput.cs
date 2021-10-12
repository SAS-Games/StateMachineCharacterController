namespace SAS.StateMachineCharacterController
{
    interface IInput
    {
        public float GetFloat(string key);
        public int GetInt(string key);
        public bool GetBool(string key);
    }
}
