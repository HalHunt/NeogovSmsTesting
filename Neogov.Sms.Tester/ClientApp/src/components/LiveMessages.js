import React from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr/dist/browser/signalr'
import { Table } from 'reactstrap';
import LocalDateTime from './LocalDateTime'
import FormattedPhoneNumber from './FormattedPhoneNumber'

const LiveMessages = props => {

    let isClosing = false;
    const [isConnected, setIsConnected] = React.useState(false);
    const [messages, setMessages] = React.useState([]);

    const setupHubConnection = () => {
        console.log('Attempting to connect to the server...');
        let connection = new HubConnectionBuilder().withUrl('/hub/messages').build();
        connection.onclose(() => {
            console.log('Connection closed.');
            if (!isClosing)
                setIsConnected(false);
        });
        connection.on("SmsReceived", message => {
            setMessages(messages => [message, ...messages]);
        });
        connection.start()
            .then(() => {
                console.log('Connection established.');
                setIsConnected(true);
            })
            .catch(error => {
                console.error(`Connection failed. ${error}`);
                setIsConnected(false);
            });
        return connection;
    }

    React.useEffect(() => {
        const connection = setupHubConnection();       
        return () => {
            isClosing = true;
            console.log('Closing connection...');
            connection.stop();
        };
    }, [])

    return (
        <div>
            <h2>Live Messages <span className={isConnected ? 'badge badge-success' : 'badge badge-danger'}>{isConnected ? 'Active' : 'Inactive'}</span></h2>
            <Table responsive striped hover>
                <thead className="thead-dark">
                    <tr>
                        <th>Created</th>
                        <th>To</th>
                        <th>From</th>
                        <th>Body</th>
                    </tr>
                </thead>
                <tbody>
                    {messages.map(message =>
                        <tr key={message.id}>
                            <td><LocalDateTime utcDateTime={message.createdUtc} /></td>
                            <td><FormattedPhoneNumber phoneNumber={message.to}></FormattedPhoneNumber></td>
                            <td><FormattedPhoneNumber phoneNumber={message.from}></FormattedPhoneNumber></td>
                            <td>{message.body}</td>
                        </tr>
                    )}
                </tbody>
            </Table>
        </div>
    );
}

export default LiveMessages