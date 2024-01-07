from maya import cmds
from maya_rubiks_animation import CubeAnimation


def main():
    my_cube_animation = CubeAnimation()

    window = cmds.window(title="Rubik's Cube Solver", iconName="Maya Rubik's", widthHeight=(300, 400), sizeable=False)
    cmds.setParent('..')
    cmds.columnLayout(adjustableColumn=True)
    cmds.button(label='Create New Cube', command=my_cube_animation.display())
    cmds.button(label='Close', command=('cmds.deleteUI(\"' + window + '\", window=True)'))

    cmds.showWindow(window)

