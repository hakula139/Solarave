namespace Play {
  public class PreciseTime {
    private int seconds;             // s
    private float accumulatedDelta;  // s
    public float Data => seconds + accumulatedDelta;
    public float DataMilli => Data * 1000f;

    public float Add(float deltaTime) {
      accumulatedDelta += deltaTime;
      while (accumulatedDelta > 1f) {
        seconds++;
        accumulatedDelta -= 1f;
      }
      return seconds + accumulatedDelta;
    }
  }
}
