import maya.api.OpenMaya as om
import maya.cmds as cmds
import maya.mel as mel


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
        print("Hello World!")

def test():
    print("Hello World!")

def create_shelf():
    if cmds.shelfLayout('MayaRubiks', exists=True):
        buttons_list = cmds.shelfLayout('MayaRubiks', query=True, childArray=True)
        try:
            cmds.deleteUI(buttons_list, control=True)
        except:
            pass
    else:
        mel.eval('addNewShelfTab %s' % "MayaRubiks")

    cmds.setParent('MayaRubiks')
    cmds.shelfButton(label="MayaRubiks", annotation="Simulate solving a rubik's cube.",
                     image1='../icons/shelf_button.png', command=test)


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
