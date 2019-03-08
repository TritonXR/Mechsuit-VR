from numpy import array, pad

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

def pad_matrix(linear_data_set, pad_length = 300):
    print('Shape prior to pad')
    print([matrix.shape for matrix in linear_data_set])
    for i in range(len(linear_data_set)):
        pad_tup = ((pad_length-linear_data_set[i].shape[0], 0), (0, 0))
        linear_data_set[i] = pad(linear_data_set[i], pad_tup,mode='mean')
    print('Shape after pad')
    print([matrix.shape for matrix in linear_data_set])
    return array(linear_data_set)
    #return [pad(matrix, (), ) for matrix in linear_data_set]