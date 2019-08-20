import React from 'react';
import FormattedPhoneNumber from './FormattedPhoneNumber'

const Home = props => {

    const [configuration, setConfiguration] = React.useState({ messageToNumber: 'unknown' });

    React.useEffect(() => {
        console.log(`Fetching configuration...`);
        fetch('api/configuration')
            .then(response => response.json())
            .then(data => {
                setConfiguration(data);
                console.log(`Configuration loaded.`);
            });
    }, [])

    return (
        <div className="jumbotron">
            <img className="d-block mx-auto" src="safari-pinned-tab.svg" alt="Chat Icon" />
            <h1>Hello, Testers!</h1>
            <p className="lead">
                This simple website was created to help you test NEOGOV's candidate text messaging feature.<br />
                Send your test text messages to: <strong><FormattedPhoneNumber phoneNumber={configuration.messageToNumber}></FormattedPhoneNumber></strong>.
            </p>
            <p>There are 2 pages to help you test.</p>
            <ul>
                <li><a href="/live">Live Messages</a> - For the # above, this page displays received SMS messages in real-time, meaning the messages appear on the page without having to manually refresh the page.</li>
                <li><a href="/messages">Messages</a> - For the # above, this page displays all received SMS messages and is searchable via the ''Created', 'To', 'From', and 'Body' fields of the message.</li>
            </ul>
            <p>If you have any questions or need any assistance please contact <a href="mailto:hblakeslee@neogov.net">Hal Blakeslee</a>.</p>
        </div>
    );
}

export default Home
