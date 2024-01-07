name_to_location_dict = {
    "top0": (-1, 1, -1),
    "top1": (0, 1, -1),
    "top2": (1, 1, -1),
    "top3": (-1, 1, 0),
    "top4": (0, 1, 0),
    "top5": (1, 1, 0),
    "top6": (-1, 1, 1),
    "top7": (0, 1, 1),
    "top8": (1, 1, 1),

    "middle0": (-1, 0, -1),
    "middle1": (0, 0, -1),
    "middle2": (1, 0, -1),
    "middle3": (-1, 0, 0),
    "middle4": (0, 0, 0),
    "middle5": (1, 0, 0),
    "middle6": (-1, 0, 1),
    "middle7": (0, 0, 1),
    "middle8": (1, 0, 1),

    "bottom0": (-1, -1, -1),
    "bottom1": (0, -1, -1),
    "bottom2": (1, -1, -1),
    "bottom3": (-1, -1, 0),
    "bottom4": (0, -1, 0),
    "bottom5": (1, -1, 0),
    "bottom6": (-1, -1, 1),
    "bottom7": (0, -1, 1),
    "bottom8": (1, -1, 1),
}

face_to_maya_geom_dict = {
    "top": [['top0.f[1]', 'top1.f[1]', 'top2.f[1]'],
            ['top3.f[1]', 'top4.f[1]', 'top5.f[1]'],
            ['top6.f[1]', 'top7.f[1]', 'top8.f[1]']],
    "bottom": [['bottom6.f[3]', 'bottom7.f[3]', 'bottom8.f[3]'],
               ['bottom3.f[3]', 'bottom4.f[3]', 'bottom5.f[3]'],
               ['bottom0.f[3]', 'bottom1.f[3]', 'bottom2.f[3]']],
    "left": [['top0.f[5]', 'top3.f[5]', 'top6.f[5]'],
             ['middle0.f[5]', 'middle3.f[5]', 'middle6.f[5]'],
             ['bottom0.f[5]', 'bottom3.f[5]', 'bottom6.f[5]']],
    "right": [['top8.f[4]', 'top5.f[4]', 'top2.f[4]'],
              ['middle8.f[4]', 'middle5.f[4]', 'middle2.f[4]'],
              ['bottom8.f[4]', 'bottom5.f[4]', 'bottom2.f[4]']],
    "front": [['top6.f[0]', 'top7.f[0]', 'top8.f[0]'],
              ['middle6.f[0]', 'middle7.f[0]', 'middle8.f[0]'],
              ['bottom6.f[0]', 'bottom7.f[0]', 'bottom8.f[0]']],
    "back": [['top2.f[2]', 'top1.f[2]', 'top0.f[2]'],
             ['middle2.f[2]', 'middle1.f[2]', 'middle0.f[2]'],
             ['bottom2.f[2]', 'bottom1.f[2]', 'bottom0.f[2]']]
}

color_to_rgb_dict = {
    "W": [1, 1, 1],
    "Y": [1, 1, 0],
    "O": [1, 0.35, 0],
    "R": [1, 0, 0],
    "G": [0, 1, 0],
    "B": [0, 0, 1]
}
