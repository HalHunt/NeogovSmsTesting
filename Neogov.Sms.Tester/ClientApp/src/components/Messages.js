import React from 'react';
import { Table } from 'reactstrap';
import LocalDateTime from './LocalDateTime'
import SearchForm from './SearchForm'

const Messages = props => {

    const [messages, setMessages] = React.useState([]);
    const [isLoading, setIsLoading] = React.useState(true);
    const [hasMoreRecords, setHasMoreRecords] = React.useState(true);
    const [currentPage, setCurrentPage] = React.useState(1);
    const [filter, setFilter] = React.useState("");

    const loadMessages = (filter, page) => {
        console.log(`Fetching received messages. filter: '${filter}', page: '${page}'...`);
        fetch(`api/message?filter=${encodeURIComponent(filter)}&page=${page}`)
            .then(response => response.json())
            .then(data => {
                if (page === 1) {
                    setMessages(data);
                }
                else {
                    setMessages([...messages, ...data]);
                }
                setIsLoading(false);
                setCurrentPage(page + 1);
                setHasMoreRecords(data.length > 0);
            });
    }

    const performSearch = filter => {
        setFilter(filter);
        loadMessages(filter, 1);
    }

    React.useEffect(() => {
        loadMessages(filter, currentPage);
    }, [])

    return (
        <div>
            <h2>Messages</h2>
            {
                isLoading === true ? <p><em>Loading...</em></p> :
                    <div>
                        <SearchForm performSearch={performSearch} />
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
                                        <td>{message.to}</td>
                                        <td>{message.from}</td>
                                        <td>{message.body}</td>
                                    </tr>
                                )}
                            </tbody>
                        </Table>
                    </div>
            }
            <button className={hasMoreRecords && !isLoading ? "btn btn-link mb-5" : "btn btn-link mb-5 invisible"} onClick={e => loadMessages(filter, currentPage)}>Load more...</button>
        </div>
    )
}

export default Messages
