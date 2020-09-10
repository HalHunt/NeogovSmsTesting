import React from 'react';
import { Form, FormGroup, Label, Button, Input } from 'reactstrap';

const SendMessage = props => {

    const [messageCount, setMessageCount] = React.useState(1);
    const [message, setMessage] = React.useState("");
    const [status, setStatus] = React.useState("Unknown");
    const [isSubmitDisabled, setIsSubmitDisabled] = React.useState(true);

    const sendMessage = event => {
        event.preventDefault();
        console.log('Sending message...');
        fetch('api/message/send',
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ count: messageCount, message: message })
            }).then(response => {
                if (response.status === 200) {
                    setStatus('Success');
                    console.log('Message sent.');
                }
                else
                    setStatus('Error');
            }).catch(error => {
                setStatus('Error');
            })
    }

    return (
        <div>
            <h2>Send a Message <span className={status === 'Unknown' ? "badge badge-warning" : status === 'Success' ? "badge badge-success" : "badge badge-danger"}>{status}</span></h2>
            <Form>
                <FormGroup>
                    <Label for="messageCount">Count</Label>
                    <Input type="number" name="messageCount" id="messageCount" placeholder="Number of messages to send" value={messageCount} onChange={event => { setMessageCount(event.target.value); setIsSubmitDisabled(!messageCount || !message); }} required />
                </FormGroup>
                <FormGroup>
                    <Label for="message">Message</Label>
                    <textarea className="form-control" name="message" id="message" placeholder="Message to send" value={message} onChange={event => { setMessage(event.target.value); setIsSubmitDisabled(!messageCount || !message); }} required maxLength="160" />
                </FormGroup>
                <Button onClick={sendMessage} disabled={isSubmitDisabled}>Send</Button>
            </Form>
        </div>
    )
}

export default SendMessage