import random
import rubiks_cube_solver


class RubiksCube:
    def __init__(self):
        self.cube = {
            "top": [["W"] * 3 for _ in range(3)],
            "bottom": [["Y"] * 3 for _ in range(3)],
            "left": [["O"] * 3 for _ in range(3)],
            "right": [["R"] * 3 for _ in range(3)],
            "front": [["G"] * 3 for _ in range(3)],
            "back": [["B"] * 3 for _ in range(3)],
        }

    def display(self):
        for face in self.cube:
            print('{face}: '.format(face=face))
            for row in self.cube[face]:
                for square in row:
                    print(square, end=" ")
                print()
            print()

    def is_solved(self):
        for face in self.cube:
            desired_color = self.cube[face][0][0]
            for row in self.cube[face]:
                for square in row:
                    if square != desired_color:
                        return False
        return True

    def rotate_cw_helper(self, face):
        self.cube[face] = list(map(list, zip(*reversed(self.cube[face]))))

    def rotate_ccw_helper(self, face):
        self.cube[face] = list(reversed(list(map(list, zip(*self.cube[face])))))

    def rotate_top_cw(self):
        self.rotate_cw_helper("top")
        temp_row = list(self.cube["front"][0])
        self.cube["front"][0] = self.cube["right"][0]
        self.cube["right"][0] = self.cube["back"][0]
        self.cube["back"][0] = self.cube["left"][0]
        self.cube["left"][0] = temp_row

    def rotate_top_ccw(self):
        self.rotate_ccw_helper("top")
        temp_row = list(self.cube["front"][0])
        self.cube["front"][0] = self.cube["left"][0]
        self.cube["left"][0] = self.cube["back"][0]
        self.cube["back"][0] = self.cube["right"][0]
        self.cube["right"][0] = temp_row

    def rotate_bottom_cw(self):
        self.rotate_cw_helper("bottom")
        temp_row = list(self.cube["front"][2])
        self.cube["front"][2] = self.cube["left"][2]
        self.cube["left"][2] = self.cube["back"][2]
        self.cube["back"][2] = self.cube["right"][2]
        self.cube["right"][2] = temp_row

    def rotate_bottom_ccw(self):
        self.rotate_ccw_helper("bottom")
        temp_row = list(self.cube["front"][2])
        self.cube["front"][2] = self.cube["right"][2]
        self.cube["right"][2] = self.cube["back"][2]
        self.cube["back"][2] = self.cube["left"][2]
        self.cube["left"][2] = temp_row

    def rotate_right_cw(self):
        self.rotate_cw_helper("right")
        for i in range(3):
            temp = self.cube["top"][i][2]
            self.cube["top"][i][2] = self.cube["front"][i][2]
            self.cube["front"][i][2] = self.cube["bottom"][i][2]
            self.cube["bottom"][i][2] = self.cube["back"][2 - i][0]
            self.cube["back"][2 - i][0] = temp

    def rotate_right_ccw(self):
        self.rotate_ccw_helper("right")
        for i in range(3):
            temp = self.cube["top"][i][2]
            self.cube["top"][i][2] = self.cube["back"][2 - i][0]
            self.cube["back"][2 - i][0] = self.cube["bottom"][i][2]
            self.cube["bottom"][i][2] = self.cube["front"][i][2]
            self.cube["front"][i][2] = temp


    def rotate_left_cw(self):
        self.rotate_cw_helper("left")
        for i in range(3):
            temp = self.cube["top"][i][0]
            self.cube["top"][i][0] = self.cube["back"][2 - i][2]
            self.cube["back"][2 - i][2] = self.cube["bottom"][i][0]
            self.cube["bottom"][i][0] = self.cube["front"][i][0]
            self.cube["front"][i][0] = temp

    def rotate_left_ccw(self):
        self.rotate_ccw_helper("left")
        for i in range(3):
            temp = self.cube["top"][i][0]
            self.cube["top"][i][0] = self.cube["front"][i][0]
            self.cube["front"][i][0] = self.cube["bottom"][i][0]
            self.cube["bottom"][i][0] = self.cube["back"][2 - i][2]
            self.cube["back"][2 - i][2] = temp

    def rotate_front_cw(self):
        self.rotate_cw_helper("front")
        for i in range(3):
            temp = self.cube["top"][2][i]
            self.cube["top"][2][i] = self.cube["left"][2 - i][2]
            self.cube["left"][2 - i][2] = self.cube["bottom"][0][2 - i]
            self.cube["bottom"][0][2 - i] = self.cube["right"][i][0]
            self.cube["right"][i][0] = temp

    def rotate_front_ccw(self):
        self.rotate_ccw_helper("front")
        for i in range(3):
            temp = self.cube["top"][2][i]
            self.cube["top"][2][i] = self.cube["right"][i][0]
            self.cube["right"][i][0] = self.cube["bottom"][0][2 - i]
            self.cube["bottom"][0][2 - i] = self.cube["left"][2 - i][2]
            self.cube["left"][2 - i][2] = temp

    def rotate_back_cw(self):
        self.rotate_cw_helper("back")
        for i in range(3):
            temp = self.cube["top"][0][i]
            self.cube["top"][0][i] = self.cube["right"][i][2]
            self.cube["right"][2 - i][2] = self.cube["bottom"][2][2 - i]
            self.cube["bottom"][2][2 - i] = self.cube["left"][2-i][0]
            self.cube["left"][i][0] = temp

    def rotate_back_ccw(self):
        self.rotate_ccw_helper("back")
        for i in range(3):
            temp = self.cube["top"][0][i]
            self.cube["top"][0][i] = self.cube["left"][2-i][0]
            self.cube["left"][2 - i][0] = self.cube["bottom"][2][2 - i]
            self.cube["bottom"][2][2 - i] = self.cube["right"][i][2]
            self.cube["right"][i][2] = temp

    move_functions_list = [rotate_top_cw, rotate_top_ccw, rotate_bottom_cw, rotate_bottom_ccw,
                           rotate_right_cw, rotate_right_ccw, rotate_left_cw, rotate_left_ccw,
                           rotate_front_cw, rotate_front_ccw, rotate_back_cw, rotate_back_ccw]

    move_string_to_function_dict = {
        "U": [rotate_top_cw],
        "U'": [rotate_top_ccw],
        "U2": [rotate_top_cw, rotate_top_cw],
        "D": [rotate_bottom_cw],
        "D'": [rotate_bottom_ccw],
        "D2": [rotate_bottom_cw, rotate_bottom_cw],
        "L": [rotate_left_cw],
        "L'": [rotate_left_ccw],
        "L2": [rotate_left_cw, rotate_left_cw],
        "R": [rotate_right_cw],
        "R'": [rotate_right_ccw],
        "R2": [rotate_right_cw, rotate_right_cw],
        "F": [rotate_front_cw],
        "F'": [rotate_front_ccw],
        "F2": [rotate_front_cw, rotate_front_cw],
        "R2": [rotate_right_cw, rotate_right_cw],
        "B": [rotate_back_cw],
        "B'": [rotate_back_ccw],
        "B2": [rotate_back_cw, rotate_back_cw],
    }

    tester = []

    def scramble(self):
        for _ in range(10):
            test = random.choice(self.move_functions_list)

            for key, value in self.move_string_to_function_dict.items():
                if test in value:
                    self.tester.append(key)
                    break

            test(self)

    def test_function(self):
        return self.tester

    def solve(self):
        move_string = rubiks_cube_solver.solve(self.cube)
        move_string_list = move_string.split(" ")
        function_list = []
        for move_string in move_string_list:
            function_list += self.move_string_to_function_dict[move_string]

        for function in function_list:
            function(self)

        self.display()

class CubeGeom(RubiksCube):
    def __init__(self):
        self.cube = {
            "top": [["top0", "top1", "top2"],
                    ["top3", "top4", "top5"],
                    ["top6", "top7", "top8"]],
            "bottom": [["bottom6", "bottom7", "bottom8"],
                       ["bottom3", "bottom4", "bottom5"],
                       ["bottom0", "bottom1", "bottom2"]],
            "left": [["top0", "top3", "top6"],
                     ["middle0", "middle3", "middle6"],
                     ["bottom0", "bottom3", "bottom6"]],
            "right": [["top8", "top5", "top2"],
                      ["middle8", "middle5", "middle2"],
                      ["bottom8", "bottom5", "bottom2"]],
            "front": [["top6", "top7", "top8"],
                      ["middle6", "middle7", "middle8"],
                      ["bottom6", "bottom7", "bottom8"]],
            "back": [["top2", "top1", "top0"],
                     ["middle2", "middle1", "middle0"],
                     ["bottom2", "bottom1", "bottom0"]]
        }


# test = CubeGeom()
# test.rotate_back_cw()
# test.display()