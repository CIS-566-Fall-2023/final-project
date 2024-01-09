import subprocess

color_to_face_dict = {
    "W": "U",
    "Y": "D",
    "O": "L",
    "R": "R",
    "G": "F",
    "B": "B"
}

face_order_list = ["top", "right", "front", "bottom", "left", "back"]


def convert_cube_to_string(cube):
    result = ""
    for face in face_order_list:
        for row in cube[face]:
            for square in row:
                result += color_to_face_dict[square]

    return result


def solve(cube):
    try:
        import SolverProgram.kociemba as kociemba
    except ImportError:
        subprocess.run(
            [
                "pip",
                "install",
                "kociemba",
                "-t",
                "./SolverProgram"
            ]
        )
        solve()

    cube_string = convert_cube_to_string(cube)

    return kociemba.solve(cube_string)
