from numpy import array

def parse_file(filename):
    data = []
    f = open(filename, "r")

    new_matrix = False
    index = -1

    for line in f:
        strip = line.split()
        if strip: # not empty line, either the beginning of new data or continuation of data
            if not new_matrix: # beginning of new data
                data.append([])
                index = index + 1
                new_matrix = True

            line_data = [float(num) for num in strip]
            data[index].append(line_data)
        else: # empty line
            new_matrix = False

    for i in range(len(data)):
        data[i] = array(data[i])
    return array(data)
