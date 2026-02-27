window.webrtc = {
    createPeerConnection: (localStream, dotNetRef, remoteUserId) => {
        const peerConnection = new RTCPeerConnection({
            iceServers: [
                { urls: "stun:stun.l.google.com:19302" }
            ]
        });

        localStream.getTracks().forEach(mediaStreamTrack =>
            peerConnection.addTrack(mediaStreamTrack, localStream));

        peerConnection.onicecandidate = e => {
            if (e.candidate) {
                dotNetRef.invokeMethodAsync(
                    "SendIceCandidate",
                    remoteUserId,
                    JSON.stringify(e.candidate)
                );
                
            }
        };
        
        peerConnection.ontrack = e => {
            let video = document.getElementById(`video-${remoteUserId}`);
            if (!video) {
                video = document.createElement("video");
                video.id = `video-${remoteUserId}`;
                video.className = "user-video"
                video.autoplay = true;
                video.playsInline = true;
                
                document.getElementById(`video-container-${remoteUserId}`).appendChild(video);
            }
            video.style.display = "flex";
            video.srcObject = e.streams[0];
        };

        return peerConnection;
    },

    createScreenPeer: (screenStream, dotNetRef, remoteUserId) => {
        const peerConnection = new RTCPeerConnection({
            iceServers: [{ urls: "stun:stun.l.google.com:19302" }]
        });
        
        screenStream.getTracks().forEach(track => peerConnection.addTrack(track, screenStream));

        peerConnection.onicecandidate = e => {
            if (e.candidate) {
                dotNetRef.invokeMethodAsync(
                    "SendScreenIceCandidate", 
                    remoteUserId, 
                    JSON.stringify(e.candidate));
            }
        };

        return peerConnection;
    },

    createScreenReceiverPeer: (dotNetRef, ownerId) => {
        const peerConnection = new RTCPeerConnection({
            iceServers: [{ urls: "stun:stun.l.google.com:19302" }]
        });

        peerConnection.ontrack = e => {
            let video = document.getElementById("screen-video");
            if(video) {
                video.srcObject = e.streams[0];
                video.style.display = "flex";
            }
        };

        peerConnection.onicecandidate = e => {
            if (e.candidate) {
                dotNetRef.invokeMethodAsync(
                    "SendScreenIceCandidate",
                    ownerId,
                    JSON.stringify(e.candidate)
                );
            }
        };

        return peerConnection;
    },

    removeScreenVideo: () =>{
        let video = document.getElementById("screen-video");
        if(video) {
            video.srcObject = null;
            video.style.display = "none";
        }
    },
    
    createOffer: async peerConnection => {
        const offer = await peerConnection.createOffer();
        await peerConnection.setLocalDescription(offer);
        return JSON.stringify(offer);
    },

    createAnswer: async peerConnection => {
        const answer = await peerConnection.createAnswer();
        await peerConnection.setLocalDescription(answer);
        return JSON.stringify(answer);
    },

    setRemoteDescription: async (peerConnection, sdpJson) => {
        const sdp = JSON.parse(sdpJson);
        await peerConnection.setRemoteDescription(new RTCSessionDescription(sdp));
    },

    addIceCandidate: async (peerConnection, candidateJson) => {
        const candidate = JSON.parse(candidateJson);
        await peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
    },

    getMediaDevices: async () => {
        const devices = await navigator.mediaDevices.enumerateDevices();
        return devices.map(device => ({
            deviceId: device.deviceId,
            kind: device.kind,
            label: device.label || `${device.kind} ${device.deviceId.substring(0, 5)}`,
            groupId: device.groupId
        }));
    },

    getUserMedia: async (videoDeviceId = null, audioDeviceId = null) => {
        const constraints = {
            video: videoDeviceId
                ? { deviceId: { exact: videoDeviceId } }
                : true,
            audio: audioDeviceId
                ? { deviceId: { exact: audioDeviceId } }
                : true
        };

        const stream = await navigator.mediaDevices.getUserMedia(constraints);

        return stream;
    },

    displayLocalMedia: stream =>{
        let video = document.getElementById(`localVideo`);
        video.autoplay = true;
        video.playsInline = true;
        
        video.srcObject = stream;
    },

    getVideoTrack: async (deviceId) => {
        const stream = await navigator.mediaDevices.getUserMedia({
            video: { deviceId: { exact: deviceId } }
        });
        return stream.getVideoTracks()[0];
    },

    getAudioTrack: async (deviceId) => {
        const stream = await navigator.mediaDevices.getUserMedia({
            audio: { deviceId: { exact: deviceId } }
        });
        return stream.getAudioTracks()[0];
    },

    replaceVideoTrack:  (stream, newTrack) => {
        const oldTrack = stream.getVideoTracks()[0];
        if (oldTrack) {
            stream.removeTrack(oldTrack);
            oldTrack.stop();
        }
        stream.addTrack(newTrack);
        
        const localVideo = document.getElementById('localVideo');
        if (localVideo) {
            localVideo.srcObject = stream;
        }
    },
    
    replaceAudioTrack: (stream, newTrack) => {
        const oldTrack = stream.getAudioTracks()[0];
        if (oldTrack) {
            stream.removeTrack(oldTrack);
            oldTrack.stop();
        }
        stream.addTrack(newTrack);
    },
    
    replaceTrackInPeer:(peerConnection, newTrack) => {
        const sender = peerConnection.getSenders()
            .find(s => s.track && s.track.kind === newTrack.kind);

        if (sender) {
            sender.replaceTrack(newTrack);
        }
    },

    toggleLocalVideo: (stream, enabled) => {
        stream.getVideoTracks().forEach(track => track.enabled = enabled);
        let video = document.getElementById(`localVideo`);
        
        video.style.display = enabled ? "flex" : "none";
    },

    toggleLocalAudio: (stream, enabled) => {
        stream.getAudioTracks().forEach(track => track.enabled = enabled);
    },

    toggleMemberVideo: (enabled, userId) => {
        let video = document.getElementById(`video-${userId}`);
        if(video) {
            video.style.display = enabled ? "flex" : "none";
        }
    },
    
    getDisplayMedia: async (dotNetRef) => {
        let stream =  await navigator.mediaDevices.getDisplayMedia({
            video: true,
            audio: true
        });

        const track = stream.getVideoTracks()[0];

        track.onended = () => {
            dotNetRef.invokeMethodAsync("StopScreenShare");
        };

        let video = document.getElementById(`screen-video`);
        if(video){
            video.srcObject = stream;
            video.style.display = "flex";
        }
        
        return stream;
    },

    replaceTrack: (peerConnection, newTrack) => {
        const sender = peerConnection.getSenders()
            .find(s => s.track.kind === newTrack.kind);
        
        if (sender) 
        {
            sender.replaceTrack(newTrack);
        }
    },

    removeUser: (peerConnection, userId) => {
        window.webrtc.closePeer(peerConnection);
        
        let video = document.getElementById(`video-${userId}`);
        if (video)
        {
            video.remove();
        }
    },

    closePeer: peerConnection => {
        if (peerConnection) 
        {
            peerConnection.close();
        }
    },

    stopStream: stream => {
        stream.getTracks().forEach(t => t.stop());
    }
};
