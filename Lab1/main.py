from tkinter import *
from tkinter import ttk

import sys

from loguru import logger

from lines import DDA
from lines import Bresenham
from lines import Wu

logger.add(sys.stderr, format="{time} {level} {message}", filter="my_module", level="INFO")
logger.add("out.log")

"""
window layout
"""
window = Tk()
window.title("Graphical Editor")
window.geometry("800x660")

"""
canvas layout
"""
canvas = Canvas(window, width=625, height=700, background="white")
canvas.grid(row=0, column=0)

frame_1 = Frame(window, background='white')
frame_1.grid(row=0, column=2)

clear_canvas_button = Button(frame_1, text="Clear canvas", background="Red")
clear_canvas_button.grid(row=3, column=2)
"""
Debug
"""
debug_frame = Frame(frame_1, background="blue")
debug_frame.grid(row=4, column=2)
debug_button = Button(debug_frame, text="Debug", background="Red")
debug_button.grid(row=0, column=0)

"""
action radiobutton frame
"""

selected_option = StringVar(value="line")

radiobutton_frame = Frame(frame_1)
radiobutton_frame.grid(row=1, column=2)

line_radiobutton = Radiobutton(radiobutton_frame, variable=selected_option, text="Line", value="line")
# circle_radiobutton = Radiobutton(radiobutton_frame, variable=selected_option, text="Circle", value="circle")

# circle_radiobutton.grid(row=0, column=1)
line_radiobutton.grid(row=0, column=2)

"""
figure frame
"""

figure_frame = Frame(frame_1)
figure_frame.grid(row=2, column=2)

"""
lines menu layout
"""
line_frame = Frame(figure_frame, highlightbackground="black", highlightthickness=1)
line_frame.grid(row=0, column=0, padx=2, pady=2)

line_label = Label(line_frame, text="Lines", font="Arial")
line_label.grid()

algorithms = ["DDA", "Bresenham", "Wu's line algorithm"]
line_box = ttk.Combobox(line_frame, values=algorithms, state="readonly")
line_box.current(0)
line_box.grid()

"""
circles layout
"""

# circle_frame = Frame(figure_frame, highlightbackground="black", highlightthickness=1)
# circle_frame.grid(row=0, column=1, padx=2, pady=2)
#
# circle_label = Label(circle_frame, text="Circles", font='Arial')
# circle_label.grid()
#
# circle_box = ttk.Combobox(circle_frame, values=algorithms, state="readonly")
# circle_box.current(0)
# circle_box.grid()


"""
events
"""

draw = list()


def figure_click(event):
    """
    Click to draw line.
    """
    if len(draw) == 2:
        draw.clear()

    logger.debug(f"radio button option: {selected_option.get()}; line algorithm: {line_box.get()}")
    logger.debug(event)

    if len(draw) == 0:
        draw.append(event)
    else:
        if draw[0].x == event.x and draw[0].y == event.y:
            return

        draw.append(event)
        points = list()
        if line_box.get() == "DDA":
            points = DDA.DDA(draw[0], draw[1])
        elif line_box.get() == "Bresenham":
            points = Bresenham.Bresenham(draw[0], draw[1])

        for i in points:
            canvas.create_rectangle(i[0], i[1], i[0] + 1, i[1] + 1, fill="black")

        if line_box.get() == "Wu's line algorithm":
            points, additional, change_flag = Wu.Wu(draw[0], draw[1])
            s1 = 1 if points[-1][0] > points[0][0] else -1
            s2 = 1 if points[-1][1] > points[0][1] else -1

            k = (points[-1][1] - points[0][1]) / (points[-1][0] - points[0][0])
            b = points[-1][1] - points[-1][0] * k
            for i in range(len(points)):
                if change_flag:
                    additional[i] = (
                        additional[i][0] - 10 * s1, additional[i][1], abs(points[i][0] * k + b - points[i][1]))
                else:
                    additional[i] = (
                        additional[i][0], additional[i][1] - 10 * s2, abs(points[i][0] * k + b - points[i][1]))

            for i in range(len(points)):
                color_1 = "#%02x%02x%02x" % (
                    abs(int(255 * additional[i][2])), abs(int(255 * additional[i][2])),
                    abs(int(255 * additional[i][2])))

                color_2 = "#%02x%02x%02x" % (
                    abs(int(255 * (1 - additional[i][2]))), abs(int(255 * (1 - additional[i][2]))),
                    abs(int(255 * (1 - additional[i][2]))))

                print("color ", color_1, len(color_1))
                canvas.create_rectangle(points[i][0], points[i][1], points[i][0] + 1, points[i][1] + 1, fill=color_1)
                canvas.create_rectangle(additional[i][0], additional[i][1], additional[i][0] + 1, additional[i][1] + 1,
                                        fill=color_2)

        logger.debug("line is drown!")


def clear_canvas(event):
    """
    Clear canvas function
    """
    draw.clear()
    logger.debug("now canvas is clear")
    canvas.delete("all")


def debug_line(event):
    if len(draw) != 2:
        return

    debug_window = Tk()
    debug_window.title("Debug")
    debug_window.geometry("600x600")

    next_button = Button(debug_window, text="Next")
    next_button.grid()

    debug_canvas = Canvas(debug_window, width=600, height=500, background="white")
    debug_canvas.grid()

    if line_box.get() == "DDA":
        points = DDA.DDA(draw[0], draw[1])
    elif line_box.get() == "Bresenham":
        points = Bresenham.Bresenham(draw[0], draw[1])

    if line_box.get() == "Wu's line algorithm":
        points, additional, change_flag = Wu.Wu(draw[0], draw[1])
        sign_x = 1
        sign_y = 1
        if additional[-1][0] - additional[0][0] < 0:
            sign_x = -1
        if additional[-1][1] - additional[0][1] < 0:
            sign_y = -1

        prev_x = additional[0][0]
        prev_y = additional[0][1]

        prev_add_x = additional[0][0]
        prev_add_y = additional[0][1]

        pixels = list(additional)
        x = 0
        y = 0
        for i in range(len(additional)):
            if pixels[i][0] == prev_x:
                additional[i] = (prev_add_x, prev_add_y, additional[i][2])
            else:
                additional[i] = (pixels[i][0] + 10 * x * sign_x, prev_add_y, additional[i][2])
                prev_add_x = pixels[i][0] + 10 * x * sign_x
                prev_x = pixels[i][0]
                x += 1

            if pixels[i][1] == prev_y:
                additional[i] = (prev_add_x, prev_add_y, additional[i][2])
            else:
                additional[i] = (prev_add_x, pixels[i][1] + 10 * y * sign_y, additional[i][2])
                prev_add_y = pixels[i][1] + 10 * y * sign_y
                prev_y = pixels[i][1]
                y += 1

    sign_x = 1
    sign_y = 1
    if points[-1][0] - points[0][0] < 0:
        sign_x = -1
    if points[-1][1] - points[0][1] < 0:
        sign_y = -1

    prev_x = points[0][0]
    prev_y = points[0][1]

    prev_add_x = points[0][0]
    prev_add_y = points[0][1]

    pixels = list(points)
    x = 0
    y = 0
    for i in range(len(points)):
        if pixels[i][0] == prev_x:
            points[i] = (prev_add_x, prev_add_y)
        else:
            points[i] = (pixels[i][0] + 10 * x * sign_x, prev_add_y)
            prev_add_x = pixels[i][0] + 10 * x * sign_x
            prev_x = pixels[i][0]
            x += 1

        if pixels[i][1] == prev_y:
            points[i] = (prev_add_x, prev_add_y)
        else:
            points[i] = (prev_add_x, pixels[i][1] + 10 * y * sign_y)
            prev_add_y = pixels[i][1] + 10 * y * sign_y
            prev_y = pixels[i][1]
            y += 1

    def debug_draw(event):
        if line_box.get() == "DDA" or line_box.get() == "Bresenham":
            debug_canvas.create_rectangle(points[0][0], points[0][1], points[0][0] + 10, points[0][1] + 10,
                                          fill="black")
            points.pop(0)
        elif line_box.get() == "Wu's line algorithm":
            color_1 = "#%02x%02x%02x" % (
                abs(int(255 * additional[0][2])), abs(int(255 * additional[0][2])),
                abs(int(255 * additional[0][2])))

            color_2 = "#%02x%02x%02x" % (
                abs(int(255 * (1 - additional[0][2]))), abs(int(255 * (1 - additional[0][2]))),
                abs(int(255 * (1 - additional[0][2]))))

            print("color ", color_1, len(color_1))
            debug_canvas.create_rectangle(points[0][0], points[0][1], points[0][0] + 10, points[0][1] + 10, fill=color_1)
            debug_canvas.create_rectangle(additional[0][0], additional[0][1], additional[0][0] + 10, additional[0][1] + 10,
                                    fill=color_2)
            points.pop(0)
            additional.pop(0)

    next_button.bind("<Button-1>", debug_draw)


"""
event bindings
"""
canvas.bind("<Button-1>", figure_click)
clear_canvas_button.bind("<Button-1>", clear_canvas)

debug_button.bind("<Button-1>", debug_line)

window.mainloop()
