def DDA(event_1, event_2):
    """
    Find pixels for raster line.
    :param event_1:
    :param event_2:
    :return: pixels (x, y) list
    """
    x_1 = event_1.x
    y_1 = event_1.y

    x_2 = event_2.x
    y_2 = event_2.y

    length = max(abs(x_1 - x_2), abs(y_1 - y_2))

    raster_unit_x = (x_2 - x_1) / length
    raster_unit_y = (y_2 - y_1) / length

    i = 0
    points = list()
    curr_x, curr_y = x_1, y_1
    while i < length:
        points.append((int(curr_x + raster_unit_x), int(curr_y + raster_unit_y)))
        curr_x = curr_x + raster_unit_x
        curr_y = curr_y + raster_unit_y
        i += 1

    print(points)
    return points
