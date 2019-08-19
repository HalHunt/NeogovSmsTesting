import React from 'react';

const LocalDateTime = props => {

    return (
        <span>{new Date(props.utcDateTime).toLocaleDateString()} {new Date(props.utcDateTime).toLocaleTimeString()}</span>
    )
}

export default LocalDateTime