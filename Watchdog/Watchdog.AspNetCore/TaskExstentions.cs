using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Watchdog.AspNetCore
{
    public static class TaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoAwait(this Task task) { }
    }
}
