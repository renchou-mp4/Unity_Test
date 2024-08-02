using System;

public static class EventExtension
{
    public static EventHandle ListenOnce(this EventDelegate del, Action action)
    {
        if (del == null)
        {
            throw new ArgumentNullException(nameof(del));
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        return del.AddListener(() =>
        {
            action.Invoke();
            del.RemoveListener(action);
        });
    }

    public static void ListenUntil(this EventDelegate del, Func<bool> action)
    {
        if (del == null)
        {
            throw new ArgumentNullException(nameof(del));
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        EventHandle handle = default;
        handle = del.AddListener(() =>
        {
            if (action.Invoke())
            {
                del.Remove(handle);
            }
        });
    }
}
