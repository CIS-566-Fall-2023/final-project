from maya import cmds
from maya_rubiks_animation import CubeAnimation, destroy_rubiks_maya_geom
from util import color_to_rgb_dict

window_name = ""
my_cube_animation = CubeAnimation()


def destroy_rubiks_ui(*args):
    global window_name
    if cmds.window(window_name, exists=True):
        cmds.deleteUI(window_name, window=True)

    for key in color_to_rgb_dict.keys():
        if cmds.objExists(key):
            cmds.delete(key, hierarchy="both")

    destroy_rubiks_maya_geom()


def show_window():
    global window_name

    window_name = cmds.window(title="Rubik's Cube Solver", iconName="Maya Rubik's", widthHeight=(300, 400))
    cmds.setParent('..')
    cmds.columnLayout(adjustableColumn=True)
    cmds.button(label='Create New Cube', command=my_cube_animation.create_ui)

    cmds.button(label='Solve', command=my_cube_animation.solve)
    cmds.button(label='Delete Cube', command=destroy_rubiks_maya_geom)

    cmds.showWindow(window_name)