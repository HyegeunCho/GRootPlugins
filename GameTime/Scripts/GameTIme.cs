using UnityEngine;

namespace GRootPlugins.GameTime
{
    public static class GameTime
    {
        public static bool IsPause { private set; get; } = false;

        public static void Pause()
        {
            IsPause = true;
        }

        public static void Play()
        {
            IsPause = false;
        }

        public static float deltaTime
        {
            get
            {
                if (IsPause) return 0f;
                return Time.deltaTime;
            }
        }
    }
}

