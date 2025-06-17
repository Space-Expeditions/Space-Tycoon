public static class EnvironmentManager
{
    public static float currentTemperature = 22f;
    public static float currentHumidity = 60f;

    public static void SetEnvironment(float temp, float humid)
    {
        currentTemperature = temp;
        currentHumidity = humid;
    }
}
