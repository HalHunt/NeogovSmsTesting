import React from 'react';

const FormattedPhoneNumber = props => {

    const phoneNumber = props.phoneNumber.trim() || null;
    switch (phoneNumber.length) {
        case 12:
            return (
                <span>{`${phoneNumber.substring(0, 2)} (${phoneNumber.substring(2, 5)}) ${phoneNumber.substring(5, 8)}-${phoneNumber.substring(8)}`}</span>
            )
        case 11:
            return (
                <span>{`+${phoneNumber.substring(0, 1)} (${phoneNumber.substring(1, 4)}) ${phoneNumber.substring(4, 7)}-${phoneNumber.substring(7)}`}</span>
            )
        case 10:
            return (
                <span>{`+1 (${phoneNumber.substring(0, 3)}) ${phoneNumber.substring(3, 6)}-${phoneNumber.substring(6)}`}</span>
            )
        default:
            return (
                <span>{phoneNumber || 'unknown'}</span>
            )
    }
}

export default FormattedPhoneNumber