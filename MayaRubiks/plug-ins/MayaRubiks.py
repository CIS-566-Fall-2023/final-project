import maya.api.OpenMaya as om
import maya.cmds as cmds
import maya.mel as mel

import importlib
import maya_rubiks_ui as ui
import maya_rubiks_animation as animation
import util
import rubiks_cube as cube_class
importlib.reload(ui)
importlib.reload(animation)
importlib.reload(util)
importlib.reload(cube_class)


def maya_useNewAPI():
    pass


class MayaRubiksCmd(om.MPxCommand):
    kPluginCmdName = "mayaRubiks"

    def __init__(self):
        om.MPxCommand.__init__(self)

    @staticmethod
    def cmdCreator():
        return MayaRubiksCmd()

    def doIt(self, args):
        ui.main()


def create_shelf():
    if cmds.shelfLayout('MayaRubiks', exists=True):
        cmds.deleteUI('MayaRubiks', control=True)

    mel.eval('addNewShelfTab %s' % "MayaRubiks")
    buttons_list = cmds.shelfLayout('MayaRubiks', query=True, childArray=True)
    try:
        cmds.deleteUI(buttons_list, control=True)
    except:
        pass

    cmds.shelfButton(label="MayaRubiks", annotation="Simulate solving a rubik's cube.",
                     image1='../icons/shelf_button.png', command=ui.show_window)


def initializePlugin(plugin):
    pluginFn = om.MFnPlugin(plugin)
    try:
        pluginFn.registerCommand(
            MayaRubiksCmd.kPluginCmdName, MayaRubiksCmd.cmdCreator
        )
    except:
        raise RuntimeError("Failed to register command")

    create_shelf()


def uninitializePlugin(plugin):
    pluginFn = om.MFnPlugin(plugin)
    try:
        pluginFn.deregisterCommand(MayaRubiksCmd.kPluginCmdName)
    except:
        raise RuntimeError("Failed to unregister command")

    ui.destroy_rubiks_ui()
    if cmds.shelfLayout('MayaRubiks', exists=True):
        cmds.deleteUI('MayaRubiks')
