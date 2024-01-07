import maya.cmds as cmds
from rubiks_cube import RubiksCube
from util import name_to_location_dict


class CubeAnimation:

    def __init__(self):
        self.my_rubiks_cube = RubiksCube()
        self.my_rubiks_cube.scramble()
        self.id = id

    def display(self):
        for key, value in name_to_location_dict:
            cmds.polyCube(n=key)
            cmds.move(value[0], value[1], value[2], key)


def main():
    pass
