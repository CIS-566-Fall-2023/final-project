import random


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
            self.cube["top"][0][i] = self.cube["right"][2 - i][2]
            self.cube["right"][2 - i][2] = self.cube["bottom"][2][2 - i]
            self.cube["bottom"][2][2 - i] = self.cube["left"][i][0]
            self.cube["left"][i][0] = temp

    def rotate_back_ccw(self):
        self.rotate_ccw_helper("back")
        for i in range(3):
            temp = self.cube["top"][0][i]
            self.cube["top"][0][i] = self.cube["left"][i][0]
            self.cube["left"][i][0] = self.cube["bottom"][2][2 - i]
            self.cube["bottom"][2][2 - i] = self.cube["right"][2 - i][2]
            self.cube["right"][2 - i][2] = temp

    move_functions_list = [rotate_top_cw, rotate_top_ccw, rotate_bottom_cw, rotate_bottom_ccw,
                           rotate_right_cw, rotate_right_ccw, rotate_left_cw, rotate_left_ccw,
                           rotate_front_cw, rotate_front_ccw, rotate_back_cw, rotate_back_ccw]

    def scramble(self):
        for _ in range(100):
            random.choice(self.move_functions_list)(self)

