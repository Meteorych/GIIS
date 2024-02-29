from tkinter import *
from tkinter import ttk

import sys

from loguru import logger

from lines import DDA
from lines import Bresenham
from lines import Wu

from circle.CircleBresenham import Circle_Bresenham
from circle.Hyperbola import hyperbola_drawing
from circle.Parabola import parabola_draw

logger.add(sys.stderr, format="{time} {level} {message}", filter="my_module", level="INFO")
logger.add("out.log")


"""
window layout
"""
window = Tk()
window.title("Graphical Editor")
window.geometry("800x660")

buttons_frame = Frame(window)
buttons_frame.grid(row=0, column=1)
"""
canvas layout
"""
canvas = Canvas(window, width=500, height=800, background="white")
canvas.grid(row=0, column=0)

clear_canvas_button = Button(buttons_frame, text="Clear canvas")
clear_canvas_button.grid(row=3, column=0)

"""
Debug
"""
debug_frame = Frame(buttons_frame)
debug_frame.grid(row=4, column=0)

debug_button = Button(debug_frame, text="Debug")
debug_button.grid(row=0, column=1)

"""
action radiobutton frame
"""

selected_option = StringVar(value="line")

radiobutton_frame = Frame(buttons_frame)
radiobutton_frame.grid(row=1, column=0)

line_radiobutton = Radiobutton(radiobutton_frame, variable=selected_option, text="Line", value="line")
circle_radiobutton = Radiobutton(radiobutton_frame, variable=selected_option, text="Circle", value="circle")
parabola_radiobutton = Radiobutton(radiobutton_frame, variable=selected_option, text="Parabola", value="parabola")
hyperbola_radiobutton = Radiobutton(radiobutton_frame, variable=selected_option, text="Hyperbola", value="hyperbola")

circle_radiobutton.grid(row=0, column=0)
line_radiobutton.grid(row=1, column=0)
parabola_radiobutton.grid(row=2, column=0)
hyperbola_radiobutton.grid(row=3, column=0)

"""
figure frame
"""

figure_frame = Frame(buttons_frame)
figure_frame.grid(row=2, column=0)

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

circle_frame = Frame(figure_frame, highlightbackground="black", highlightthickness=1, background="green")
circle_frame.grid(row=0, column=1, padx=2, pady=2)

circle_label = Label(circle_frame, text="Ellipse", font='Arial', background="green")
circle_label.grid()

circle_box = ttk.Combobox(circle_frame, values=['Bresenham', 'Circle'], state="readonly")
circle_box.current(0)
circle_box.grid()

"""
parabola layout
"""

parabola_frame = Frame(figure_frame, highlightbackground="black", highlightthickness=1, background="green")
parabola_frame.grid(row=1, column=0, padx=2, pady=2)

parabola_label = Label(parabola_frame, text="Parabola", font='Arial', background="green")
parabola_label.grid()

parabola_box = ttk.Combobox(parabola_frame, values=['Bresenham'], state="readonly")
parabola_box.current(0)
parabola_box.grid()

"""
hyperbola layout
"""
hyperbola_frame = Frame(figure_frame, highlightbackground="black", highlightthickness=1)
hyperbola_frame.grid(row=1, column=1, padx=2, pady=2)

hyperbola_label = Label(hyperbola_frame, text="Hyperbola", font='Arial')
hyperbola_label.grid()

hyperbola_box = ttk.Combobox(hyperbola_frame, values=['Bresenham'], state="readonly")
hyperbola_box.current(0)
hyperbola_box.grid()

"""
events
"""

draw = list()


def figure_click(event):
    logger.debug(f"radio button option: {selected_option.get()}")
    logger.debug(event)
    if len(draw) == 2:
        draw.clear()

    draw.append(event)
    if len(draw) == 1:
        return
    if selected_option.get() == 'line':
        logger.debug(f"line algorithm {line_box.get()}")
        line_click(event)
    elif selected_option.get() == 'circle':
        logger.debug(f"line algorithm {circle_box.get()}")
        circle_click(event)
    elif selected_option.get() == 'parabola':
        parabola_click(event)
    elif selected_option.get() == 'hyperbola':
        hyperbola_click(event)


def hyperbola_click(event):
    pixels = hyperbola_drawing(draw[0], draw[1])

    for i in pixels:
        canvas.create_rectangle(i[0], i[1], i[0] + 1, i[1] + 1, fill="black")


def parabola_click(event):
    pixels = parabola_draw(draw[0], draw[1])

    for i in pixels:
        canvas.create_rectangle(i[0], i[1], i[0] + 1, i[1] + 1, fill="black")


def circle_click(event):
    if circle_box.get() == 'Circle':
        pixels = Circle_Bresenham(draw[0], draw[1], circle=True)
    else:
        pixels = Circle_Bresenham(draw[0], draw[1])

    for i in pixels:
        canvas.create_rectangle(i[0], i[1], i[0] + 1, i[1] + 1, fill="black")


def line_click(event):
    """
    Click to draw line.
    """

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


def choose_debug(event):
    logger.debug(f"radio button option: {selected_option.get()}; line algorithm: {line_box.get()}")
    logger.debug(event)

    if selected_option.get() == 'line':
        debug_line(event)
    elif selected_option.get() == 'circle':
        debug_circle(event)
    elif selected_option.get() == 'parabola':
        debug_parabola(event)
    elif selected_option.get() == 'hyperbola':
        debug_hyperbola(event)


def debug_hyperbola(event):
    if len(draw) != 2:
        return
    debug_window = Tk()
    debug_window.title("Debug")
    debug_window.geometry("1000x1000")

    next_button = Button(debug_window, text="Next")
    next_button.grid()

    debug_canvas = Canvas(debug_window, width=1000, height=1000, background="white")
    debug_canvas.grid()

    pixels = []
    pixels = hyperbola_drawing(draw[0], draw[1])

    def draw_point(event):
        debug_canvas.create_rectangle(pixels[0][0], pixels[0][1], pixels[0][0], pixels[0][1],
                                      fill='black')
        logger.debug(f"{pixels[0][0]}, {pixels[0][1]}")
        pixels.pop(0)
        print(pixels)

    next_button.bind("<Button-1>", draw_point)

def debug_parabola(event):
    if len(draw) != 2:
        return
    debug_window = Tk()
    debug_window.title("Debug")
    debug_window.geometry("1000x1000")

    next_button = Button(debug_window, text="Next")
    next_button.grid()

    debug_canvas = Canvas(debug_window, width=1000, height=1000, background="white")
    debug_canvas.grid()

    pixels = []
    pixels = parabola_draw(draw[0], draw[1])

    def draw_point(event):
        debug_canvas.create_rectangle(pixels[0][0], pixels[0][1], pixels[0][0], pixels[0][1],
                                      fill='black')
        logger.debug(f"{pixels[0][0]}, {pixels[0][1]}")
        pixels.pop(0)
        print(pixels)

    next_button.bind("<Button-1>", draw_point)


def debug_circle(event):
    if len(draw) != 2:
        return
    debug_window = Tk()
    debug_window.title("Debug")
    debug_window.geometry("1000x1000")

    next_button = Button(debug_window, text="Next")
    next_button.grid()

    debug_canvas = Canvas(debug_window, width=1000, height=1000, background="white")
    debug_canvas.grid()

    pixels = []
    if circle_box.get() == 'Circle':
        pixels = Circle_Bresenham(draw[0], draw[1], circle=True)
    else:
        pixels = Circle_Bresenham(draw[0], draw[1])

    def draw_point(event):
        debug_canvas.create_rectangle(pixels[0][0], pixels[0][1], pixels[0][0], pixels[0][1],
                                      fill='black')
        logger.debug(f"{pixels[0][0]}, {pixels[0][1]}")
        pixels.pop(0)
        print(pixels)

    next_button.bind("<Button-1>", draw_point)


def debug_line(event):
    if len(draw) != 2:
        return

    debug_window = Tk()
    debug_window.title("Debug")
    debug_window.geometry("1000x1000")

    next_button = Button(debug_window, text="Next")
    next_button.grid()

    debug_canvas = Canvas(debug_window, width=1000, height=1000, background="white")
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

            debug_canvas.create_rectangle(points[0][0], points[0][1], points[0][0] + 10, points[0][1] + 10,
                                          fill=color_1)
            debug_canvas.create_rectangle(additional[0][0], additional[0][1], additional[0][0] + 10,
                                          additional[0][1] + 10,
                                          fill=color_2)
            points.pop(0)
            additional.pop(0)

    next_button.bind("<Button-1>", debug_draw)


"""
event bindings
"""
canvas.bind("<Button-1>", figure_click)
clear_canvas_button.bind("<Button-1>", clear_canvas)

debug_button.bind("<Button-1>", choose_debug)

window.mainloop()