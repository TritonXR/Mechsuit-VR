def parse_file(filename):
    data = []
    f = open(filename, "r")

    new_matrix = False
    index = -1

    for line in f:
        strip = line.split()
        if strip:
            if not new_matrix:
                data.append([])
                index = index + 1
                new_matrix = True

            line_data = [float(num) for num in strip]
            data[index].append(line_data)
        else:
            new_matrix = False

    return data
