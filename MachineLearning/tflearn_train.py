"""
https://www.programcreek.com/python/example/105174/tflearn.lstm
"""
import tflearn
import numpy as np
from numpy import array
from dataParser import parse_file, pad_matrix
import os

def build_net(layers, high_length, hidden_nodes, maxlen, char_idx, dropout = False):
    net = tflearn.input_data([None, high_length, maxlen]) # Why Keras works but this doesn't
    for n in range(layers - 1):
        net = tflearn.lstm(incoming = net, n_units = hidden_nodes, return_seq = True)
        if dropout:
            net = tflearn.dropout(net, 0.5)
    
    net = tflearn.lstm(net, hidden_nodes)
    if dropout:
        net = tflearn.dropout(net, 0.5)
    
    net = tflearn.fully_connected(net, char_idx, activation='softmax')
    net = tflearn.regression(net, optimizer = 'adam', loss = 'categorical_crossentropy')
    return net
    
if __name__ == '__main__':
    # Get file names 
    class_names = [filename.strip(".txt") for filename in os.listdir("Data/Training")]
    data_set = [parse_file("Data/Training/" + filename + ".txt") for filename in class_names]

    linear_data_set = []
    linear_label_set = []
    index = 0
    for data in data_set:
        for matrix in data:
            linear_data_set.append(matrix)
            linear_label_set.append(index)
        index = index + 1

    linear_data_set = pad_matrix(linear_data_set)
    linear_label_set = array(linear_label_set)
    print('linear_label_set.shape:', linear_label_set.shape)
    print('linear_data_set.shape:', linear_data_set.shape)
    print('linear_data_set[0].shape:', linear_data_set[0].shape)
    net = build_net(3, 300, 32, 6, 3, True)
    model = tflearn.DNN(net, tensorboard_verbose=0)
    model.fit(linear_data_set, linear_label_set, n_epoch=5)

    #Test
    # Get file names
    class_names = [filename.strip(".txt") for filename in os.listdir("Data/Test")]
    data_set = [parse_file("Data/Test/" + filename + ".txt") for filename in class_names]

    linear_data_set = []
    linear_label_set = []
    index = 0
    for data in data_set:
        for matrix in data:
            linear_data_set.append(matrix)
            linear_label_set.append(index)
        index = index + 1

    linear_data_set = pad(array(linear_data_set), 300)

    
    for i in range(len(linear_data_set)):
        pass
        predictions = model.predict(array([linear_data_set[i]]))
        print(str(np.argmax(predictions[0])) + " " +  str([linear_label_set[i]]))
    
    model.save('motions.tflearn')