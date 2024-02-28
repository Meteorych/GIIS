def draw_hyperbola(event_1, event_2):
    a = abs(event_2.x - event_1.x) // 2
    b = abs(event_2.y - event_1.y) // 2
    h = (event_1.x + event_2.x) // 2
    k = (event_1.y + event_2.y) // 2
    x = 0
    y = b
    d = b * b - a * a * b + a * a / 4

    pixels = []
    while (a * a * (2 * y - 1) > 2 * b * b * (x + 1)):
        if (d < 0):
            d = d + b * b * (2 * x + 3)
            x = x + 1
        else:
            d = d + b * b * (2 * x + 3) + a * a * (-2 * y + 2)
            x = x + 1
            y = y - 1
        pixels.append((x + h, -y + k))
        pixels.append((-x + abs(event_1.x - event_2.x) + h, y - abs(event_1.y - event_2.y) + k))

    d = b * b * (x + 1) * (x + 1) + a * a * (y - 1) * (y - 1) - a * a * b * b

    while (y > 0):
        if (d < 0):
            d = d + b * b * (2 * x + 2) + a * a * (-2 * y + 3)
            x = x + 1
            y = y - 1
        else:
            d = d + a * a * (-2 * y + 3)
            y = y - 1
        pixels.append((x + h, -y + k))
        pixels.append((-x + abs(event_1.x - event_2.x) + h, y - abs(event_1.y - event_2.y) + k))

    return pixels