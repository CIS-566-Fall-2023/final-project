import maya.cmds as cmds
import maya.mel as mel

from rubiks_cube import RubiksCube, CubeGeom
from util import name_to_location_dict, face_to_maya_geom_dict, color_to_rgb_dict


def destroy_rubiks_maya_geom(*args):
    for key in name_to_location_dict.keys():
        if cmds.objExists(key):
            cmds.delete(key, hierarchy="both")

    if cmds.objExists('pfxToon1'):
        cmds.delete('pfxToon1', hierarchy="both")


def create_shader(name, node_type="aiStandardSurface"):
    material = cmds.shadingNode(node_type, name=name, asShader=True)
    sg = cmds.sets(name="%sSG" % name, empty=True, renderable=True, noSurfaceShader=True)
    cmds.connectAttr("%s.outColor" % material, "%s.surfaceShader" % sg)
    return material, sg


def add_keyframes(time):
    cmds.select(clear=True)
    for key in name_to_location_dict:
        cmds.select(key, add=True)

    cmds.setKeyframe(at='rotate', t=['{}sec'.format(time), '{}sec'.format(time+1)])

    cmds.select(clear=True)


class CubeAnimation:
    color_shaders = {}

    def __init__(self):
        self.my_rubiks_cube = RubiksCube()
        self.my_rubiks_cube.scramble()

        self.my_cube_geom = CubeGeom()

        self.curr_time = 0

        for key, value in color_to_rgb_dict.items():
            new_mtl, new_sg = create_shader(key)
            cmds.setAttr(new_mtl + ".baseColor", value[0], value[1], value[2], type="double3")
            self.color_shaders[key] = (new_mtl, new_sg)

    def display_initial(self, _):
        destroy_rubiks_maya_geom()

        self.my_rubiks_cube = RubiksCube()
        self.my_rubiks_cube.scramble()

        self.my_cube_geom = CubeGeom()

        self.curr_time = 0

        cube_names_list = []
        for key, value in name_to_location_dict.items():
            cube_names_list.append(cmds.polyCube(n=key))
            cmds.move(value[0], value[1], value[2], key)
            cmds.move(0, 0, 0, key + ".rotatePivot")

        [cmds.select(cube_name, add=True) for cube_name in cube_names_list]
        mel.eval("assignNewPfxToon;")

        for key, value in color_to_rgb_dict.items():
            if not cmds.objExists(key):
                new_mtl, new_sg = create_shader(key)
                cmds.setAttr(new_mtl + ".baseColor", value[0], value[1], value[2], type="double3")
                self.color_shaders[key] = (new_mtl, new_sg)

        for face, row_list in self.my_rubiks_cube.cube.items():
            for row_idx, row in enumerate(row_list):
                for square_idx, square in enumerate(row):
                    found_geom = face_to_maya_geom_dict[face][row_idx][square_idx]
                    cmds.sets(found_geom, forceElement=self.color_shaders[square][1])

        add_keyframes(self.curr_time)

        cmds.select(clear=True)

    def rotate_top_cw(self, _):
        self.my_rubiks_cube.rotate_top_cw()
        self.my_cube_geom.rotate_top_cw()

        squares_in_grp = list()
        for row in self.my_cube_geom.cube["top"]:
            for square in row:
                squares_in_grp.append(square)

        cmds.select(clear=True)

        for square in squares_in_grp:
            cmds.select(square, add=True)

        cmds.rotate(0, -90, 0, r=True)

        cmds.select(clear=True)

        self.curr_time += 3
        add_keyframes(self.curr_time)
