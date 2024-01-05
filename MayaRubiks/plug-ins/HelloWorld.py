import maya.OpenMaya as OpenMaya
import maya.OpenMayaMPx as OpenMayaMPx

class HelloWorld(OpenMayaMPx.MPxCommand):
        
    def doIt(self, argList):
        print("Hello World!")
        
def creator():
    return OpenMayaMPx.asMPxPtr(HelloWorld())

def createMyShelf():
	shelfName = 'My_Shelf'
	test = cmds.shelfLayout(shelfName, ex=True)
	if test:
		# If the shelf already exists, clear the contents and re-add the buttons.
		newShelf = shelfName
		buttons = cmds.shelfLayout(newShelf, query=True, childArray=True)
		cmds.deleteUI(buttons, control=True)
	else:
		newShelf = mel.eval('addNewShelfTab %s' % shelfName)
	cmds.setParent(newShelf)

def initializePlugin(obj):
    plugin = OpenMayaMPx.MFnPlugin(obj, "Chad Vernon", "1.0", "Any")
    try:
        plugin.registerCommand("helloWorld", creator)
    except:
        raise RuntimeError("Failed to register command")

def uninitializePlugin(obj):
    plugin = OpenMayaMPx.MFnPlugin(obj)
    try:
        plugin.deregisterCommand("helloWorld")
    except:
        raise RuntimeError("Failed to unregister command")
