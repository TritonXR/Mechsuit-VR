import tensorflow as tf
from tensorflow import keras

import os
from dataParser import parse_file

if __name__ == '__main__':
    class_names = [filename.strip(".txt") for filename in os.listdir("Data")]
    data_set = [parse_file("Data/" + filename + ".txt") for filename in class_names]

    linear_data_set = []
    linear_label_set = []
    index = 0
    for data in data_set:
        for matrix in data:
            linear_data_set.append(matrix)
            linear_label_set.append(index)
        index = index + 1

    model = keras.Sequential([
        # TODO change input_shape first arg to None
        keras.layers.Flatten(input_shape=(10, 6)),
        keras.layers.Dense(128, activation = tf.nn.relu),
        keras.layers.Dense(3, activation = tf.nn.softmax)
    ])

    model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])
    model.fit(linear_data_set, linear_label_set, epochs = 5)

    saved_model_path = tf.contrib.saved_model.save_keras_model(model, 'Models')
