import tensorflow as tf
from tensorflow import keras

import numpy as np
from numpy import array

import os
from dataParser import parse_file

from freeze_session import freeze_session

if __name__ == '__main__':
	# Get file names 
    class_names = [filename.strip(".txt") for filename in os.listdir("Data/Training")]
    data_set = [parse_file("Data/Training/" + filename + ".txt") for filename in class_names]

    # TODO convert to list comprehension
	#linear_data_set = [matrix for matrix in data for data in data_set]
    # This would not work :(
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

    model = keras.Sequential([
        keras.layers.LSTM(32, return_sequences=True, input_shape=(None, 6)),
        keras.layers.Dropout(0.2),
        keras.layers.LSTM(32, return_sequences=True),
        keras.layers.Dropout(0.2),
        keras.layers.LSTM(32),
        keras.layers.Dense(3, activation = tf.nn.softmax)
    ])

    model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])
    for i in range(len(linear_data_set)):
        model.fit(array(linear_data_set[i]), [linear_label_set[i]], epochs = 5)


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
    model.save('motions.h5')
    #model.save_weights('./checkpoints/my_checkpoint')
	
	# First way: get session and save, tweak params of freeze_model.py
	# Get the Tensorflow Session
    #tSess = tf.keras.backend.get_session()
	# Save checkpoint file
    #tSaver = tf.train.Saver()
    #tSaver.save(tSess, "./Profile.ckpt") # It will generate 4 files: checkpoint, Profile.ckpt.index, Profile.ckpt.meta, Profile.ckpt.data-00000-of-00001

    # Save pb file
    #tf.train.write_graph(tSess.graph_def, "./", 'Profile.pb', as_text=True)

	#Second way: using custom freeze_session
    #frozen_graph = freeze_session(tSess, output_names=[out.op.name for out in model.outputs])
    #tf.train.write_graph(frozen_graph, '', 'Train.pb', as_text = False)