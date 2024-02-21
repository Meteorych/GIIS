def Bresenham(event_1, event_2):
    """
    Find pixels for raster line.
    :param event_1:
    :param event_2:
    :return: pixels (x, y) list
    """

    x = event_1.x
    y = event_1.y

    dx = abs(event_2.x - event_1.x)
    dy = abs(event_2.y - event_1.y)

    points = []

    s1 = 1 if event_2.x > event_1.x else -1
    s2 = 1 if event_2.y > event_1.y else -1

    if dy > dx:
        dx, dy = dy, dx
        change_flag = True
    else:
        change_flag = False

    e = 2 * dy - dx

    for _ in range(dx + 1):
        points.append((x, y))
        while e >= 0:
            if change_flag:
                x += s1
            else:
                y += s2
            e -= 2 * dx
        if change_flag:
            y += s2
        else:
            x += s1
        e += 2 * dy

    return points
