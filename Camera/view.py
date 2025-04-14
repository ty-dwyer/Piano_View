import cv2
import cv2.aruco as aruco
import numpy as np

# List of dictionaries to check
aruco_dicts = [
    aruco.DICT_4X4_50,
    aruco.DICT_4X4_100,
    aruco.DICT_4X4_250,
    aruco.DICT_4X4_1000,
    aruco.DICT_5X5_50,
    aruco.DICT_5X5_100,
    aruco.DICT_5X5_250,
    aruco.DICT_5X5_1000,
    aruco.DICT_6X6_50,
    aruco.DICT_6X6_100,
    aruco.DICT_6X6_250,
    aruco.DICT_6X6_1000,
    aruco.DICT_7X7_50,
    aruco.DICT_7X7_100,
    aruco.DICT_7X7_250,
    aruco.DICT_7X7_1000,
    aruco.DICT_ARUCO_ORIGINAL,
]

parameters = aruco.DetectorParameters()

# Start video capture from the default webcam
cap = cv2.VideoCapture(0)

if not cap.isOpened():
    print("Could not open webcam")
    exit()

print("Press 'q' to quit.")

while True:
    ret, frame = cap.read()
    if not ret:
        print("Failed to grab frame")
        break

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    all_corners = []

    # Detect markers for all dictionaries
    for dict_type in aruco_dicts:
        dictionary = aruco.getPredefinedDictionary(dict_type)
        corners, ids, rejectedImgPoints = aruco.detectMarkers(
            gray, dictionary, parameters=parameters
        )
        if ids is not None:
            aruco.drawDetectedMarkers(frame, corners, ids)
            all_corners.extend(corners)

    cv2.imshow("ArUco Marker Detection", frame)

    if cv2.waitKey(1) & 0xFF == ord("q"):
        break

cap.release()
cv2.destroyAllWindows()
