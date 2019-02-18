import tensorflow as tf
from tensorflow import keras

import numpy as np
from numpy import array

import os
from dataParser import parse_file


if __name__ == '__main__':
	# Get file names 
    class_names = [filename.strip(".txt") for filename in os.listdir("Data/Training")]
    data_set = [parse_file("Data/Training/" + filename + ".txt") for filename in class_names]

    # TODO convert to list comprehension
	#linear_data_set = [matrix for matrix in data for data in data_set]
    # This would not work 😢
	#linear_label_set = [index for index in range(len(data)) for data in data_set]
    linear_data_set = []
    linear_label_set = []
    index = 0
    for data in data_set:
        for matrix in data:
            linear_data_set.append(matrix)
            linear_label_set.append(index)
        index = index + 1

    linear_data_set = array(linear_data_set)
    #linear_data_set.reshape(linear_data_set.size, None, 6)
    #print(linear_data_set)
    #print(linear_data_set.shape)
    model = keras.Sequential([
        keras.layers.LSTM(32, input_shape=(None, 6)),
        keras.layers.Dense(3, activation = tf.nn.softmax)
    ])

    model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])
    for i in range(len(linear_data_set)):
        model.fit(array([linear_data_set[i]]), [linear_label_set[i]], epochs = 5)


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

    linear_data_set = array(linear_data_set)

    for i in range(len(linear_data_set)):
        predictions = model.predict(array([linear_data_set[i]]))
        print(str(np.argmax(predictions[0])) + " " +  str([linear_label_set[i]]))

    #saved_model_path = tf.contrib.saved_model.save_keras_model(model, 'Models')
