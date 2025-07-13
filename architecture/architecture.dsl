workspace "Name" "Description"

    !identifiers hierarchical

    model {
        u = person "Anonnymous User"
        ss = softwareSystem "Mini Ticket System" {
            wa = container "SPA Angular" {
                tags "WebApp"
                ticketForm = component "Ticket Form Component"
                ticketList = component "Ticket List Component"
                ticketService = component "Ticket Service"
                errorInterceptor = component "Error Interceptor" {
                    technology "HttpInterceptor"
                }
            }

            api = container ".NET Core Api" {
                tags "API"
                middleware = component "ErrorHandleMiddleware" {
                    technology ".NET Core Middleware"
                    description "Catches unhandled exceptions and returns standardized error responses"
                }
                ticketscontroller = component "TicketsController"
                statusescontroller = component "StatusesController"
                service = component "TicketService"
                repository = component "TicketContext" {
                    technology "In-memory EF Core DbContext"
                }
            }

            db = container "InMemory Database" {
                tags "Database"
            }
        }

        u -> ss.wa "Uses"
        ss.wa.ticketForm -> ss.wa.ticketService "Creates \ Edits ticket"
        ss.wa.ticketList -> ss.wa.ticketService "Fetches tickets"
        ss.wa.ticketService -> ss.wa.errorInterceptor "Prepare HTTP requests"
        ss.wa.errorInterceptor -> ss.api.middleware "Makes HTTP request"


        ss.api.middleware -> ss.api.ticketscontroller "Forwards request if no error"
        ss.api.middleware -> ss.api.statusescontroller "Forwards request if no error"
        ss.api.ticketscontroller -> ss.api.service "Delegates business logic"
        ss.api.statusescontroller -> ss.api.repository "Reads statueses (!)"
        ss.api.service -> ss.api.repository "Reads/writes data"
        ss.api.repository -> ss.db "In-memory DB access"
    }

    views {
        systemContext ss "Diagram1" {
            include *
            autolayout lr
        }

        container ss "Diagram2" {
            include *
            autolayout lr
        }

        component ss.wa "AngularAppComponents" {
            include *
            autolayout lr
        }

        component ss.api "APIComponents" {
            include *
            autolayout lr
        }

        styles {
            element "Element" {
                color #0773af
                stroke #0773af
                strokeWidth 7
                shape roundedbox
            }
            element "Person" {
                shape person
            }
            element "Database" {
                shape cylinder
            }
            element "WebApp" {
                background #e3f2fd
            }
            element "API" {
                background #fce4ec
            }
            element "Boundary" {
                strokeWidth 5
            }
            relationship "Relationship" {
                thickness 4
            }
        }
    }

}
